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
 * @property Collection|Material[] $materials
 * @property User $createdBy
 * @property Date $created_at
 */
class ProjectTemplate extends Model
{
    protected $guarded = [];

    public function team()
    {
        return $this->belongsTo(Team::class);
    }

    public function materials()
    {
        return $this->belongsToMany(Material::class)->withPivot('quantity');
    }

    public function createdBy()
    {
        return $this->belongsTo(User::class, 'created_by_id');
    }
}
