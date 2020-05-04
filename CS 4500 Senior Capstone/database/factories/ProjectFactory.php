<?php

use App\Models\Project;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(Project::class, function (Faker $faker) {
    return [
        'name'              => $faker->word,
        'description'       => $faker->text,
        'address'           => $faker->streetAddress,
        'city'              => $faker->city,
        'state'             => $faker->randomElement(['Florida', 'Utah', 'California', 'Texas']),
        'zip'               => $faker->postcode,
        'contact_name'      => $faker->name,
        'contact_phone'     => $faker->phoneNumber,
        'contact_email'     => $faker->email,
        'sq_foot'           => $faker->numberBetween(1000, 10000),
        'labor_hours'       => $faker->randomFloat(2, 100, 1000),
        'labor_rate'        => $faker->randomFloat(2, 100, 1000),
        'percentage_on_top' => $faker->randomFloat(0, 0, 15),
    ];
});
