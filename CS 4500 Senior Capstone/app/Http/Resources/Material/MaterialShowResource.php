<?php

namespace App\Http\Resources\Material;

use App\Models\Material;
use Illuminate\Http\Resources\Json\JsonResource;

class MaterialShowResource extends JsonResource
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
            'project_templates' => $this->projectTemplates,
            'quantity'          => optional($this->pivot)->quantity,
            'created_at'        => $this->created_at->format('F d, Y'),
        ];
    }
}
