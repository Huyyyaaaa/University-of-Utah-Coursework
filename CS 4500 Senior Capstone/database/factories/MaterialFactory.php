<?php

use App\Models\Material;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(Material::class, function (Faker $faker) {
    return [
        'name'             => $faker->word,
        'description'      => $faker->text,
        'price'            => $faker->numberBetween(1000, 10000),
        'sq_foot_coverage' => $faker->numberBetween(10, 500),
    ];
});
