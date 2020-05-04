<?php

namespace App\Http\Controllers;

use App\Http\Resources\Task\TaskShowResource;
use App\Models\Task;

class TaskController extends Controller
{
    public function __invoke()
    {
        $this->authorize('viewAny', Task::class);

        return view('task.index', [
            'tasks' => TaskShowResource::collection(user()->tasks),
        ]);
    }
}
