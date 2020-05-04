<?php

use App\Models\Task;
use Faker\Generator as Faker;
use Illuminate\Database\Eloquent\Factory;

/* @var $factory Factory */
$factory->define(Task::class, function (Faker $faker) {
    return [
        'description' => $faker->text,
        'due_at'      => $faker->dateTimeBetween('-1 month', '+1 month'),
        'completed'   => $faker->boolean,
    ];
});
