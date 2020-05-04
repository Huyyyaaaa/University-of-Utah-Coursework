<?php

use App\Models\ProjectTemplate;
use App\Models\Team;
use Illuminate\Database\Seeder;

class ProjectTemplatesTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        factory(ProjectTemplate::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Commercial Sarnafil',
            'description'   => 'Sarnafil roof',
            'created_by_id' => $team->users->random(),
        ]);
        factory(ProjectTemplate::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Commercial Tar and Gravel',
            'description'   => 'Tar and gravel roof',
            'created_by_id' => $team->users->random(),
        ]);
        factory(ProjectTemplate::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Residential Shingle',
            'description'   => 'Shingle roof',
            'created_by_id' => $team->users->random(),
        ]);
        factory(ProjectTemplate::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Residential Sarnafil',
            'description'   => 'Sarnafil roof',
            'created_by_id' => $team->users->random(),
        ]);
        factory(ProjectTemplate::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Residential Metal',
            'description'   => 'Metal roof',
            'created_by_id' => $team->users->random(),
        ]);
    }
}
