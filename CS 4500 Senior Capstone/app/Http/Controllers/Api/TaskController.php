<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\Task\TaskShowResource;
use App\Models\Project;
use App\Models\Task;

class TaskController extends Controller
{
    public function index(Project $project = null)
    {
        if ($project) {
            $this->authorize('viewAnyByProject', [Task::class, $project]);
        } else {
            $this->authorize('viewAny', Task::class);
        }

        return $project ? TaskShowResource::collection($project->tasks) : TaskShowResource::collection(user()->tasks);
    }

    public function store()
    {
        if (request('project_id')) {
            $this->authorize('createByProject', [Task::class, Project::query()->find(request('project_id'))]);
        } else {
            $this->authorize('create', Task::class);
        }

        $attributes         = $this->validateRequest();
        $computedAttributes = [
            'team_id'       => team()->id,
            'created_by_id' => user()->id,
            'due_at'        => request('due_at') ? carbon(request('due_at')) : null,
        ];
        $result             = array_merge($attributes, $computedAttributes);

        $task = user()->tasks()->create($result);
        return $this->success('Task Successfully Created', ['task' => TaskShowResource::make($task)]);
    }

    public function update(Task $task)
    {
        if (request('project_id')) {
            $this->authorize('updateByProject', [$task, Project::query()->find(request('project_id'))]);
        } else {
            $this->authorize('update', $task);
        }

        $attributes = $this->validateRequest();

        unset($attributes['due_at']);
        $due_at = request('due_at') ? carbon(request('due_at')) : null;

        $task->update(array_merge($attributes, ['due_at' => $due_at]));


        return $this->success('Task Successfully Updated', ['task' => TaskShowResource::make($task)]);
    }

    public function destroy(Task $task)
    {
        $this->authorize('delete', $task);

        $task->delete();
        return $this->success('Task Successfully Deleted');
    }

    public function incompleteCount()
    {
        return user()->tasks()->where('completed', '=', false)->count();
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'project_id'  => 'nullable|numeric',
            'description' => 'required',
            'completed'   => 'required',
            'due_at'      => 'required',
        ]);

        return $attributes;
    }
}
