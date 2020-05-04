<?php

namespace App\Policies;

use App\Models\Project;
use App\Models\Task;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class TaskPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user)
    {
        return true;
    }

    public function viewAnyByProject(User $user, Project $project)
    {
        return $user->onTeam($project->team);
    }

    public function create(User $user)
    {
        return true;
    }

    public function createByProject(User $user, Project $project)
    {
        return $user->onTeam($project->team);
    }

    public function update(User $user, Task $task)
    {
        return $user->onTeam($task->team) && $task->isCreatedByUser($user);
    }

    public function updateByProject(User $user, Task $task, Project $project)
    {
        return $user->onTeam($project->team) && $task->isCreatedByUser($user);
    }

    public function delete(User $user, Task $task)
    {
        return $user->onTeam($task->team) && $task->isCreatedByUser($user);
    }
}
