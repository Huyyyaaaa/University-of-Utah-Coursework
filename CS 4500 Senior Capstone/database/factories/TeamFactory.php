<?php

use App\Models\Team;
use App\Models\User;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/** @var Factory $factory */
$factory->define(Team::class, function (Faker $faker) {
    return [
        'owner_id'   => User::query()->inRandomOrder()->first(),
        'name'       => $faker->name,
        'address'    => $faker->streetAddress,
        'city'       => $faker->city,
        'state'      => $faker->randomElement(['Florida', 'Utah', 'California', 'Texas']),
        'zip'        => $faker->postcode,
        'phone'      => $faker->phoneNumber,
        'team_email' => $faker->unique()->safeEmail,
        'disclaimer' => $faker->sentence,
    ];
});
