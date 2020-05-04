<?php

namespace App\Policies;

use App\Models\Project;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class ProjectPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user)
    {
        return true;
    }

    public function view(User $user, Project $project)
    {
        return $user->onTeam($project->team);
    }

    public function create(User $user)
    {
        return true;
    }

    public function update(User $user, Project $project)
    {
        return $user->onTeam($project->team);
    }

    public function delete(User $user, Project $project)
    {
        return $user->onTeam($project->team) && $user->isManagerOrAbove();
    }
}
