<?php

namespace App\Policies;

use App\Models\Material;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class MaterialPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user)
    {
        return true;
    }

    public function view(User $user, Material $material)
    {
        return $user->onTeam($material->team);
    }

    public function create(User $user)
    {
        return $user->isManagerOrAbove();
    }

    public function update(User $user, Material $material)
    {
        return $user->onTeam($material->team) && $user->isManagerOrAbove();
    }

    public function delete(User $user, Material $material)
    {
        return $user->onTeam($material->team) && $user->isManagerOrAbove();
    }
}
