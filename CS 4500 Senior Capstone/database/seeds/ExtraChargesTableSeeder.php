<?php

use App\Models\ExtraCharge;
use App\Models\Team;
use Illuminate\Database\Seeder;

class ExtraChargesTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projects()->each(function ($project) use ($team) {
            $extraCharge = factory(ExtraCharge::class, 1)->create([
                'team_id'     => $team->id,
                'project_id'  => $project->id,
                'description' => 'Tractor Rental',
                'amount'      => 600.00,
            ]);

            $extraCharge = $extraCharge->first();
            $project->bids()->each(function ($bid) use ($team, $extraCharge) {
                factory(ExtraCharge::class, 1)->create([
                    'team_id'     => $team->id,
                    'bid_id'      => $bid->id,
                    'description' => $extraCharge->description,
                    'amount'      => $extraCharge->amount,
                ]);
                $bid->update(['price' => $bid->price + $extraCharge->amount]);
            });
        });
    }
}
