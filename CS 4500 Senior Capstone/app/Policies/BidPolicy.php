<?php

namespace App\Policies;

use App\Models\Bid;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class BidPolicy
{
    use HandlesAuthorization;

//    public function viewAny(User $user)
//    {
//        return $user->isManagerOrAbove();
//    }

    public function view(User $user, Bid $bid)
    {
        return $user->onTeam($bid->team);
    }

    public function create(User $user)
    {
        return true;
    }

    public function update(User $user, Bid $bid)
    {
        return $user->onTeam($bid->team) && $user->isManagerOrAbove();
    }

    public function delete(User $user, Bid $bid)
    {
        return $user->onTeam($bid->team) && $user->isManagerOrAbove();
    }
}
