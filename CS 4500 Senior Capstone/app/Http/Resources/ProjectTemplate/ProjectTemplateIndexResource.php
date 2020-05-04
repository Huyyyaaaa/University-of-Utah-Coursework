<?php

namespace App\Http\Resources\ProjectTemplate;

use App\Http\Resources\Material\MaterialShowResource;
use App\Models\ProjectTemplate;
use Illuminate\Http\Resources\Json\JsonResource;

class ProjectTemplateIndexResource extends JsonResource
{
    public function toArray($request)
    {
        /** @var ProjectTemplate $this */
        return [
            'id'          => $this->id,
            'team'        => $this->team,
            'name'        => $this->name,
            'description' => $this->description,
            'created_by'  => $this->createdBy,
            'materials'   => MaterialShowResource::collection($this->materials),
            'created_at'  => $this->created_at->format('F d, Y'),
        ];
    }
}
