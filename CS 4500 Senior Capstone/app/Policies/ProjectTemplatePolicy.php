<?php

namespace App\Policies;

use App\Models\ProjectTemplate;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class ProjectTemplatePolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user)
    {
        return true;
    }

    public function view(User $user, ProjectTemplate $projectTemplate)
    {
        return $user->onTeam($projectTemplate->team);
    }

    public function create(User $user)
    {
        return $user->isManagerOrAbove();
    }

    public function update(User $user, ProjectTemplate $projectTemplate)
    {
        return $user->onTeam($projectTemplate->team) && $user->isManagerOrAbove();
    }

    public function delete(User $user, ProjectTemplate $projectTemplate)
    {
        return $user->onTeam($projectTemplate->team) && $user->isAdminOrAbove();
    }
}
