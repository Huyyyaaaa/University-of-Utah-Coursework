<?php

namespace App\Http\Resources\Material;

use App\Models\Material;
use Illuminate\Http\Resources\Json\JsonResource;

class MaterialIndexResource extends JsonResource
{
    public function toArray($request)
    {
        /** @var Material $this */
        return [
            'id'                => $this->id,
            'team'              => $this->team,
            'name'              => $this->name,
            'description'       => $this->description,
            'price'             => $this->price,
            'sq_foot_coverage'  => $this->sq_foot_coverage,
            'created_by'        => $this->createdBy,
            'created_at'        => $this->created_at->format('F d, Y'),
        ];
    }
}
