<?php

use App\Models\Bid;
use App\Models\Team;
use Illuminate\Database\Seeder;

class BidsTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        $team->projects()->each(function ($project) use ($team) {
            $total = 0;
            $project->materials()->each(function ($material) use (&$total) {
                $total += $material->pivot->quantity * $material->price;
            });
            $total += $total * $project->percentage_on_top / 100;
            $total += $project->labor_hours * $project->labor_rate;
            $project->extraCharges()->each(function ($extraCharge) use (&$total) {
                $total += $extraCharge->amount;
            });
            factory(Bid::class, 1)->create([
                'name'              => $project->name . ' First Bid',
                'description'       => 'First bid generated for ' . $project->name,
                'team_id'           => $team->id,
                'project_id'        => $project->id,
                'address'           => $project->address,
                'city'              => $project->city,
                'state'             => $project->state,
                'zip'               => $project->zip,
                'contact_name'      => $project->contact_name,
                'contact_phone'     => $project->contact_phone,
                'contact_email'     => $project->contact_email,
                'sq_foot'           => $project->sq_foot,
                'labor_hours'       => $project->labor_hours,
                'labor_rate'        => $project->labor_rate,
                'percentage_on_top' => $project->percentage_on_top,
                'price'             => $total,
                'created_by_id'     => $team->users->random(),
                'won'               => true,
            ]);
        });
    }
}
