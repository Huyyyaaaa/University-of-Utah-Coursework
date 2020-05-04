<?php

use App\Models\ProjectTemplate;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(ProjectTemplate::class, function (Faker $faker) {
    return [
        'name' => $faker->word,
        'description' => $faker->text,
    ];
});
