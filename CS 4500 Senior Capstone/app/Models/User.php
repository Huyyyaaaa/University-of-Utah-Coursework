<?php

namespace App\Models;

use Laravel\Passport\HasApiTokens;
use Laravel\Spark\CanJoinTeams;
use Laravel\Spark\User as SparkUser;

/**
 * @property string name
 */
class User extends SparkUser
{
    use CanJoinTeams, HasApiTokens;
    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
        'name',
        'email',
    ];

    /**
     * The attributes excluded from the model's JSON form.
     *
     * @var array
     */
    protected $hidden = [
        'password',
        'remember_token',
        'authy_id',
        'country_code',
        'phone',
        'two_factor_reset_code',
        'card_brand',
        'card_last_four',
        'card_country',
        'billing_address',
        'billing_address_line_2',
        'billing_city',
        'billing_zip',
        'billing_country',
        'extra_billing_information',
    ];

    /**
     * The attributes that should be cast to native types.
     *
     * @var array
     */
    protected $casts = [
        'trial_ends_at'        => 'datetime',
        'uses_two_factor_auth' => 'boolean',
    ];

    public function isAdminOrAbove()
    {
        $role = $this->roleOnCurrentTeam();
        return $role === 'admin' || $role === 'owner';
    }

    public function isManagerOrAbove()
    {
        $role = $this->roleOnCurrentTeam();
        return $role === 'manager' || $role === 'admin' || $role === 'owner';
    }

    public function tasks()
    {
        return $this->hasMany(Task::class, 'created_by_id');
    }
}
