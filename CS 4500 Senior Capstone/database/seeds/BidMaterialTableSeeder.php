<?php

use App\Models\Team;
use Illuminate\Database\Seeder;

class BidMaterialTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projects()->each(function ($project) use ($team) {
            $materials = $project->materials;
            $bid       = $project->bids()->first();
            foreach ($materials as $material) {
                $bid->materials()->attach($material, ['quantity' => 1]);
            }
        });
    }
}
