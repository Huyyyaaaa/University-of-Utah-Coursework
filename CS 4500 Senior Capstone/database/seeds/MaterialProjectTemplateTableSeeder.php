<?php

use App\Models\Team;
use Illuminate\Database\Seeder;

class MaterialProjectTemplateTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projectTemplates()->each(function ($projectTemplate) use ($team) {
            $materials = $team->materials()->inRandomOrder()->limit(2)->get();
            $projectTemplate->materials()->attach($materials, ['quantity' => rand(1, 10)]);
        });
    }
}
