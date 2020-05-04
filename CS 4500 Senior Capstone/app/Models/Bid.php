<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Collection;
use Illuminate\Support\Facades\Date;
use phpDocumentor\Reflection\Types\Boolean;

/**
 * @property int $id
 * @property Team $team
 * @property string $name
 * @property string $description
 * @property string $address
 * @property string $city
 * @property string $state
 * @property string $zip
 * @property string $contact_name
 * @property string $contact_phone
 * @property string $contact_email
 * @property int sq_foot
 * @property int labor_hours
 * @property int labor_rate
 * @property int percentage_on_top
 * @property int $price
 * @property Project $project
 * @property Collection|Material[] $materials
 * @property Collection|ExtraCharge[] $extraCharges
 * @property User $createdBy
 * @property Boolean $won
 * @property Date $created_at
 */
class Bid extends Model
{
    protected $guarded = [];

    public function team()
    {
        return $this->belongsTo(Team::class);
    }

    public function project()
    {
        return $this->belongsTo(Project::class);
    }

    public function materials()
    {
        return $this->belongsToMany(Material::class)->withPivot('quantity');
    }

    public function createdBy()
    {
        return $this->belongsTo(User::class, 'created_by_id');
    }

    public function extraCharges()
    {
        return $this->hasMany(ExtraCharge::class);
    }
}
