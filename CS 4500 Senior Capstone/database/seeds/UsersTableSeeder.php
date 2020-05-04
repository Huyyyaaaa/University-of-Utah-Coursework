<?php

use App\Models\Team;
use App\Models\User;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\File;
use Illuminate\Support\Facades\Storage;

class UsersTableSeeder extends Seeder
{
    public function run()
    {
        $this->createDeveloperAccount();
        $this->createRoofersTeam();
    }

    private function createDeveloperAccount()
    {
        Storage::disk('public')->makeDirectory('profiles');

        File::copy(
            __DIR__ . '/photos/logo.png',
            storage_path('/app/public/profiles/logo.png')
        );

        $dev = factory(User::class)->create([
            'name'      => 'Developer',
            'email'     => 'dev@dev.com',
            'password'  => bcrypt('secret'),
            'photo_url' => Storage::disk('public')->url('profiles/logo.png'),
        ]);

        $devTeam = factory(Team::class)->create([
            'owner_id'   => $dev->id,
            'name'       => 'Developer Team',
            'photo_url'  => Storage::disk('public')->url('profiles/logo.png'),
            'address'    => '201 Presidents\' Cir',
            'city'       => 'Salt Lake City',
            'state'      => 'Utah',
            'zip'        => '84112',
            'phone'      => '801-581-7200',
            'team_email' => 'admissions@utah.edu',
            'disclaimer' => 'Developer Team provides estimates and has the right to change any part of the estimate.',
        ]);
        $devTeam->users()->attach($dev, ['role' => 'owner']);
    }

    private function createRoofersTeam()
    {
        Storage::disk('public')->makeDirectory('profiles');

        File::copy(
            __DIR__ . '/photos/logo.png',
            storage_path('/app/public/profiles/logo.png')
        );

        $teamRoofers = factory(Team::class)->create([
            'name'       => 'Raise the Roof',
            'owner_id'   => 1,
            'photo_url'  => Storage::disk('public')->url('profiles/logo.png'),
            'address'    => '201 Presidents\' Cir',
            'city'       => 'Salt Lake City',
            'state'      => 'Utah',
            'zip'        => '84112',
            'phone'      => '801-581-7200',
            'team_email' => 'admissions@utah.edu',
            'disclaimer' => 'Raise the Roof provides estimates and has the right to change any part of the estimate.',
        ]);

        $this->attachUser(['name' => 'Jake', 'email' => 'jake@jake.com'], $teamRoofers, 'owner', 'jake.jpeg');
        $this->attachUser(['name' => 'Aaron Templeton', 'email' => 'aaron@aaron.com'], $teamRoofers, 'admin', 'aaron.png');
        $this->attachUser(['name' => 'Barrett Nield', 'email' => 'barrett@barrett.com'], $teamRoofers, 'admin', 'barrett.jpg');
        $this->attachUser(['name' => 'Wyatt Davis', 'email' => 'wyatt@wyatt.com'], $teamRoofers, 'admin', 'wyatt.jpg');
        $this->attachUser(['name' => 'Batman', 'email' => 'batman@batman.com'], $teamRoofers, 'manager', 'batman.png');
        $this->attachUser(['name' => 'Robin', 'email' => 'robin@robin.com'], $teamRoofers, 'employee', 'robin.png');
    }

    private function attachUser(array $array, Team $teamRoofers, $role, $picture)
    {
        File::copy(
            __DIR__ . '/photos/' . $picture,
            storage_path('/app/public/profiles/' . $picture)
        );

        $user = factory(User::class)->create(array_merge($array, [
            'photo_url' => Storage::disk('public')->url('profiles/' . $picture),
            'password'  => bcrypt('secret'),
        ]));

        if ($role == 'owner') {
            $teamRoofers->update(['owner_id' => $user->id]);
        }

        $teamRoofers->users()->attach($user, ['role' => $role]);
    }
}
