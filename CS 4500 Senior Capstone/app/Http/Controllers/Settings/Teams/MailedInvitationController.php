<?php

namespace App\Http\Controllers\Settings\Teams;

use App\Http\Controllers\Controller;
use Laravel\Spark\Contracts\Interactions\Settings\Teams\SendInvitation;
use App\Http\Requests\Settings\Teams\CreateInvitationRequest;
use Laravel\Spark\Invitation;
use Laravel\Spark\Spark;

class MailedInvitationController extends Controller
{
    public function store(CreateInvitationRequest $request, $team)
    {
        Spark::interact(SendInvitation::class, [$team, $request->email, $request->role]);
    }

    public function destroy(Invitation $invitation)
    {
        //Todo change to policy
        abort_unless(user()->isAdminOrAbove(), 403);
        $invitation->delete();
    }
}
