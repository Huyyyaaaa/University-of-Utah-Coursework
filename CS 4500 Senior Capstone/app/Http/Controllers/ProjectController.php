<?php

namespace App\Http\Controllers;

use App\Http\Resources\Project\ProjectShowResource;
use App\Models\Project;

class ProjectController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', Project::class);

        return view('project.index');
    }

    public function create()
    {
        $this->authorize('create', Project::class);

        return view('project.create');
    }

    public function show(Project $project)
    {
        $this->authorize('view', $project);

        return view('project.show', [
            'project' => ProjectShowResource::make($project)
        ]);
    }
}
