<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\Bid\BidIndexResource;
use App\Http\Resources\Bid\BidShowResource;
use App\Models\Bid;
use App\Models\User;
use App\Notifications\BidCreated;
use Illuminate\Support\Facades\Notification;

class BidController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', Bid::class);

        return BidIndexResource::collection(team()->bids()->orderByDesc('created_at')->get());
    }

    public function store()
    {
        $this->authorize('create', Bid::class);

        $attributes         = $this->validateRequest();
        $computedAttributes = [
            'price'         => $this->calculateTotal(),
            'created_by_id' => user()->id,
        ];
        $result             = array_merge($attributes, $computedAttributes);

        /** @var Bid $bid */
        $bid = team()->bids()->create($result);
        $this->syncMaterials($bid);
        $this->syncExtraCharges($bid);

        $managerOrAboveTeamMembers = team()->users->filter(function ($value) {
            /** @var User $value */
            return $value->isManagerOrAbove();
        });
        Notification::send($managerOrAboveTeamMembers, new BidCreated(user(), $bid));

        return $this->success('Bid Successfully Created', ['bid' => BidShowResource::make($bid)]);
    }

    public function update(Bid $bid)
    {
        $this->authorize('update', $bid);

        $attributes = request()->validate([
            'won' => 'required',
        ]);

        $bid->update($attributes);

        return $this->success('Bid Successfully Updated', ['bid' => BidShowResource::make($bid)]);
    }

    public function destroy(Bid $bid)
    {
        $this->authorize('delete', $bid);

        $bid->delete();
        return $this->success('Bid Successfully Deleted');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'project_id'        => 'required|numeric',
            'name'              => 'required',
            'description'       => 'required',
            'address'           => 'required',
            'city'              => 'required',
            'state'             => 'required',
            'zip'               => 'required',
            'contact_name'      => 'required',
            'contact_phone'     => 'required',
            'contact_email'     => 'required',
            'sq_foot'           => 'required|numeric',
            'labor_hours'       => 'required|numeric',
            'labor_rate'        => 'required|numeric',
            'percentage_on_top' => 'required|numeric',
            'won'               => 'required',
        ]);

        return $attributes;
    }

    private function calculateTotal()
    {
        $materialTotal    = $this->calculateMaterialTotal();
        $addedOnTop       = $materialTotal * (request('percentage_on_top') / 100);
        $labor            = request('labor_hours') * request('labor_rate');
        $extraChargeTotal = $this->calculateExtraChargesTotal();

        return $materialTotal + $addedOnTop + $labor + $extraChargeTotal;
    }

    private function calculateMaterialTotal()
    {
        $materials = collect(request('materials'));
        $total     = 0;
        $materials->each(function ($material) use (&$total) {
            $total += $material['price'] * $material['quantity'];
        });
        return $total;
    }

    private function calculateExtraChargesTotal()
    {
        $extraCharges = collect(request('adjustments'));
        $total        = 0;
        $extraCharges->each(function ($extraCharge) use (&$total) {
            $total += $extraCharge['amount'];
        });
        return $total;
    }

    private function syncMaterials(Bid $bid)
    {
        $materials = collect(request('materials'));
        $materials = $materials->mapWithKeys(function ($material) {
            return [
                $material['id'] => ['quantity' => $material['quantity']]
            ];
        });
        $bid->materials()->sync($materials);
    }

    private function syncExtraCharges(Bid $bid)
    {
        $extraCharges = collect(request('adjustments'));
        $extraCharges->each(function ($extraCharge) use ($bid, &$total) {
            $bid->extraCharges()->create([
                'team_id'     => team()->id,
                'description' => $extraCharge['description'],
                'amount'      => $extraCharge['amount'],
            ]);
        });
    }
}
