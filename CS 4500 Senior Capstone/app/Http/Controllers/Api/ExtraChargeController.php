<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\ExtraChargeResource;
use App\Models\ExtraCharge;
use App\Models\Project;

class ExtraChargeController extends Controller
{
    public function store(Project $project)
    {
        $this->authorize('update', $project);
        $attributes = $this->validateRequest();

        $extraCharge = $project->extraCharges()->create(array_merge($attributes, ['team_id' => team()->id]));

        return $this->success('Extra Charge Successfully Created!', [
            'extraCharge' => ExtraChargeResource::make($extraCharge),
        ]);
    }

    public function update(Project $project, ExtraCharge $extraCharge)
    {
        $this->authorize('update', $project);
        $attributes = $this->validateRequest();

        $extraCharge->update($attributes);

        return $this->success('Extra Charge Successfully Updated!', [
            'extraCharge' => ExtraChargeResource::make($extraCharge),
        ]);
    }

    public function destroy(Project $project, ExtraCharge $extraCharge)
    {
        $this->authorize('update', $project);

        $extraCharge->delete();

        return $this->success('Extra Charge Successfully Deleted!');
    }

    private function validateRequest()
    {
        return request()->validate([
            'description' => 'required',
            'amount'      => 'required',
        ]);
    }
}
