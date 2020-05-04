<?php

use App\Models\ExtraCharge;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(ExtraCharge::class, function (Faker $faker) {
    return [
        'description' => $faker->sentence,
        'amount'      => $faker->randomNumber(2),
    ];
});
