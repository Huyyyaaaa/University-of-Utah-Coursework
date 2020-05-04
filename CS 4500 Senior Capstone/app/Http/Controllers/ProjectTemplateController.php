<?php

namespace App\Http\Controllers;

use App\Http\Resources\ProjectTemplate\ProjectTemplateShowResource;
use App\Models\ProjectTemplate;

class ProjectTemplateController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', ProjectTemplate::class);

        return view('project-template.index');
    }

    public function create()
    {
        $this->authorize('create', ProjectTemplate::class);

        return view('project-template.create');
    }

    public function show(ProjectTemplate $project_template)
    {
        $this->authorize('view', $project_template);

        return view('project-template.show', [
            'project_template' => ProjectTemplateShowResource::make($project_template)
        ]);
    }
}
