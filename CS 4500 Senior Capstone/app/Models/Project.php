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
 * @property string $address
 * @property string $city
 * @property string $state
 * @property string $zip
 * @property string $contact_name
 * @property string $contact_phone
 * @property string $contact_email
 * @property int sq_foot
 * @property Collection|Material[] $materials
 * @property Collection|ExtraCharge[] $extraCharges
 * @property Collection|Bid[] $bids
 * @property int labor_hours
 * @property int labor_rate
 * @property int percentage_on_top
 * @property User $createdBy
 * @property Date $created_at
 * @property Collection|Task[] $tasks
 * @property Collection|Photo[] $photos
 */
class Project extends Model
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

    public function bids()
    {
        return $this->hasMany(Bid::class);
    }

    public function createdBy()
    {
        return $this->belongsTo(User::class, 'created_by_id');
    }

    public function extraCharges()
    {
        return $this->hasMany(ExtraCharge::class);
    }

    public function tasks()
    {
        return $this->hasMany(Task::class);
    }

    public function photos()
    {
        return $this->hasMany(Photo::class);
    }
}
