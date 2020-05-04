<?php

use App\Models\Bid;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(Bid::class, function (Faker $faker) {
    return [
        'name'        => $faker->word,
        'description' => $faker->text,
        'price'       => $faker->randomNumber(2),
        'won'         => $faker->boolean,
        'created_at'  => $faker->dateTimeBetween('-2 months', 'now'),
    ];
});
