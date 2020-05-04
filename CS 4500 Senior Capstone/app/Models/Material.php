<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Collection;
use Illuminate\Support\Facades\Date;

/**
 * @property int $id
 * @property Team $team
 * @property string $name
 * @property string $description
 * @property int $price
 * @property int $sq_foot_coverage
 * @property Collection|ProjectTemplate[] $projectTemplates
 * @property Collection|Project[] $projects
 * @property Collection|Bid[] $bids
 * @property User $createdBy
 * @property Date $created_at
 * @property Collection|Photo[] $photos
 */
class Material extends Model
{
    protected $guarded = [];

    public function team()
    {
        return $this->belongsTo(Team::class);
    }

    public function projectTemplates()
    {
        return $this->belongsToMany(ProjectTemplate::class);
    }

    public function projects()
    {
        return $this->belongsToMany(Project::class)->withPivot('quantity');
    }

    public function bids()
    {
        return $this->belongsToMany(Bid::class)->withPivot('quantity');
    }

    public function createdBy()
    {
        return $this->belongsTo(User::class, 'created_by_id');
    }

    public function photos()
    {
        return $this->hasMany(Photo::class);
    }
}
