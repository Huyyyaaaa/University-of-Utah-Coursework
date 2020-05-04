<?php

namespace App\Http\Controllers\Settings\Teams;

use App\Http\Controllers\Controller;
use App\Models\Team;

class TeamInformationController extends Controller
{
    public function update(Team $team)
    {
        $attributes = $this->validateRequest();

        $team->update($attributes);

        return $this->success('Team Information Successfully Updated!');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'address'    => 'nullable',
            'city'       => 'nullable',
            'state'      => 'nullable',
            'zip'        => 'nullable',
            'phone'      => 'nullable',
            'team_email' => 'nullable|email',
            'disclaimer' => 'nullable',
        ]);

        return $attributes;
    }
}
