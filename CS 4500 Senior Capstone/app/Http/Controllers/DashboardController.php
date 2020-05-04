<?php

namespace App\Http\Controllers;

use App\Http\Resources\Task\TaskShowResource;

class DashboardController extends Controller
{
    public function show()
    {
        $tasks = user()->tasks;
        return view('dashboard', [
            'tasks' => TaskShowResource::collection($tasks)
        ]);
    }
}
