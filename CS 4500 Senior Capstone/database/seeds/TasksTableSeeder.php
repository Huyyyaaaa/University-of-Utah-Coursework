<?php

use App\Models\Task;
use App\Models\Team;
use Illuminate\Database\Seeder;

class TasksTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projects()->each(function ($project) use ($team) {
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'project_id'    => $project->id,
                'created_by_id' => $team->users->random(),
                'description'   => 'Get pictures of job site and upload to project.',
            ]);
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'project_id'    => $project->id,
                'created_by_id' => $team->users->random(),
                'description'   => 'Generate and send bid to client for review.',
            ]);
        });

        $team->users()->each(function ($user) use ($team) {
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'created_by_id' => $user->id,
                'description'   => 'Import list of materials from The Home Depot to system.',
            ]);
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'created_by_id' => $user->id,
                'description'   => 'Create new template for basic roof repair job.',
            ]);
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'created_by_id' => $user->id,
                'description'   => 'Create new project for the Warnock Engineering Building at the University of Utah.',
            ]);
            factory(Task::class, 1)->create([
                'team_id'       => $team->id,
                'created_by_id' => $user->id,
                'description'   => 'Talk to Jerry about potential leads.',
            ]);
        });
    }
}
