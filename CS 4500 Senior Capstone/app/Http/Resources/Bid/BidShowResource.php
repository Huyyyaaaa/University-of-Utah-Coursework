<?php

namespace App\Http\Resources\Bid;

use App\Http\Resources\ExtraChargeResource;
use App\Http\Resources\Material\MaterialShowResource;
use App\Models\Bid;
use Illuminate\Http\Resources\Json\JsonResource;

class BidShowResource extends JsonResource
{
    public function toArray($request)
    {
        /** @var Bid $this */
        return [
            'id'                => $this->id,
            'project'           => $this->project,
            'team'              => $this->team,
            'name'              => $this->name,
            'description'       => $this->description,
            'address'           => $this->address,
            'city'              => $this->city,
            'state'             => $this->state,
            'zip'               => $this->zip,
            'contact_name'      => $this->contact_name,
            'contact_phone'     => $this->contact_phone,
            'contact_email'     => $this->contact_email,
            'sq_foot'           => $this->sq_foot,
            'labor_hours'       => $this->labor_hours,
            'labor_rate'        => $this->labor_rate,
            'percentage_on_top' => $this->percentage_on_top,
            'price'             => $this->price,
            'created_by'        => $this->createdBy,
            'materials'         => MaterialShowResource::collection($this->materials),
            'created_at'        => $this->created_at->format('F d, Y'),
            'extra_charges'     => ExtraChargeResource::collection($this->extraCharges),
            'won'               => $this->won,
        ];
    }
}
