<?php

namespace App\Http\Resources;

use App\Models\ExtraCharge;
use Illuminate\Http\Resources\Json\JsonResource;

class ExtraChargeResource extends JsonResource
{
    public function toArray($request)
    {
        /** @var ExtraCharge $this */
        return [
            'id'          => $this->id,
            'team'        => $this->team,
            'project'     => $this->project,
            'bid'         => $this->bid,
            'description' => $this->description,
            'amount'      => $this->amount,
        ];
    }
}
