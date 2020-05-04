<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\ProjectTemplate\ProjectTemplateIndexResource;
use App\Http\Resources\ProjectTemplate\ProjectTemplateShowResource;
use App\Models\ProjectTemplate;

class ProjectTemplateController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', ProjectTemplate::class);

        return ProjectTemplateIndexResource::collection(team()->projectTemplates);
    }

    public function store()
    {
        $this->authorize('create', ProjectTemplate::class);

        $attributes         = $this->validateRequest();
        $computedAttributes = [
            'created_by_id' => user()->id,
        ];
        $result             = array_merge($attributes, $computedAttributes);

        $projectTemplate = team()->projectTemplates()->create($result);
        $this->syncMaterials($projectTemplate);
        return $this->success('Project Template Successfully Created',
            ['project_template' => ProjectTemplateShowResource::make($projectTemplate)]);
    }

    public function update(ProjectTemplate $projectTemplate)
    {
        $this->authorize('update', $projectTemplate);

        $attributes = $this->validateRequest();
        $projectTemplate->update($attributes);
        $this->syncMaterials($projectTemplate);
        return $this->success('Project Template Successfully Updated',
            ['project_template' => ProjectTemplateShowResource::make($projectTemplate)]);
    }

    public function destroy(ProjectTemplate $projectTemplate)
    {
        $this->authorize('delete', $projectTemplate);

        $projectTemplate->delete();
        return $this->success('Project Template Successfully Deleted');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'name'        => 'required',
            'description' => 'required',
        ]);

        return $attributes;
    }

    private function syncMaterials(ProjectTemplate $projectTemplate)
    {
        $materials = collect(request('materials'));
        $materials = $materials->mapWithKeys(function ($material) {
            return [
                $material['id'] => ['quantity' => $material['quantity']]
            ];
        });
        $projectTemplate->materials()->sync($materials);
    }
}
