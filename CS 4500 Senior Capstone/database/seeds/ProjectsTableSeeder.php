<?php

use App\Models\Project;
use App\Models\Team;
use Illuminate\Database\Seeder;

class ProjectsTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'Warnock Engineering Building',
            'description'       => 'New roof',
            'address'           => '72 Central Campus Dr. Room 1650 WEB',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Richard B. Brown',
            'contact_phone'     => '801-581-6911',
            'contact_email'     => 'contact@coe.utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'Merril Engineering Building',
            'description'       => 'New building',
            'address'           => '50 S. Central Campus Dr. Room 2110 MEB',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Richard B. Brown',
            'contact_phone'     => '801-581-6911',
            'contact_email'     => 'contact@coe.utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'A. Ray Olpin University Union',
            'description'       => 'Clean all windows',
            'address'           => '200 S Central Campus Dr.',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801.581.7200',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'J. Willard Marriott Library',
            'description'       => 'Clean grounds',
            'address'           => '295 S 1500 E',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801.581.8558',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'Kingsbury Hall',
            'description'       => 'New chairs',
            'address'           => '1395 E. Presidents circle',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801-581-7100',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'Libby Gardner Concert Hall',
            'description'       => 'New Building',
            'address'           => '1375 E. Presidents circle',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801-581-6762',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'George S Eccles Student Life Center',
            'description'       => 'New equipment',
            'address'           => '1836 Student Life Way',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801-581-8898',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
        factory(Project::class, 1)->create([
            'team_id'           => $team->id,
            'created_by_id'     => $team->users->random(),
            'name'              => 'Financial Aid Boat Building',
            'description'       => 'New building',
            'address'           => '201 SOUTH 1460 EAST',
            'city'              => 'Salt Lake City',
            'state'             => 'Utah',
            'zip'               => '84112',
            'contact_name'      => 'Ruth Watkins',
            'contact_phone'     => '801-581-6211',
            'contact_email'     => 'addmissions@utah.edu',
        ]);
    }
}
