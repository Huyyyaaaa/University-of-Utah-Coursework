<?php

namespace App\Models;

use Illuminate\Support\Collection;
use Laravel\Spark\Team as SparkTeam;

/**
 * Class Team
 * @package App\Models
 * @property Collection|Material[] $materials
 * @property Collection|ProjectTemplate[] $projectTemplates
 * @property Collection|Project[] $projects
 * @property Collection|Bid[] $bid
 */
class Team extends SparkTeam
{
    protected $fillable = [
        'name',
        'address',
        'city',
        'state',
        'zip',
        'team_email',
        'phone',
        'disclaimer',
        'slug'
    ];

    public function materials()
    {
        return $this->hasMany(Material::class);
    }

    public function projectTemplates()
    {
        return $this->hasMany(ProjectTemplate::class);
    }

    public function projects()
    {
        return $this->hasMany(Project::class);
    }

    public function bids()
    {
        return $this->hasMany(Bid::class);
    }
}
