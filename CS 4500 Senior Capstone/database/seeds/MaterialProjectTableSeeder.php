<?php

use App\Models\Team;
use Illuminate\Database\Seeder;

class MaterialProjectTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projects()->each(function ($project) use ($team) {
            $materials = $team->materials()->inRandomOrder()->limit(5)->get();
            foreach ($materials as $material) {
                $project->materials()->attach($material, ['quantity' => 1]);
            }
        });
    }
}
