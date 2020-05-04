<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Date;

/**
 * @property int $id
 * @property Team $team
 * @property Project $project
 * @property string $description
 * @property Date $due_at
 * @property Boolean $completed
 * @property User $createdBy
 * @property Date $created_at
 */
class Task extends Model
{
    protected $guarded = [];
    protected $dates = [
        'due_at',
    ];

    public function team()
    {
        return $this->belongsTo(Team::class);
    }

    public function project()
    {
        return $this->belongsTo(Project::class);
    }

    public function createdBy()
    {
        return $this->belongsTo(User::class, 'created_by_id');
    }

    public function isCreatedByUser(User $user)
    {
        return $this->createdBy->id === $user->id;
    }
}
