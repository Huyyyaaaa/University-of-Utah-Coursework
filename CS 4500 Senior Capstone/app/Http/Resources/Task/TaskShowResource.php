<?php

namespace App\Http\Resources\Task;

use App\Models\Task;
use Illuminate\Http\Resources\Json\JsonResource;

class TaskShowResource extends JsonResource
{
    public function toArray($request)
    {
        /** @var Task $this */
        return [
            'id'          => $this->id,
            'team'        => $this->team,
            'project'     => $this->project,
            'description' => $this->description,
            'due_at'      => $this->due_at->format('F d, Y, H:i'),
            'completed'   => $this->completed,
            'created_by'  => $this->createdBy,
            'created_at'  => $this->created_at->format('F d, Y'),
        ];
    }
}
