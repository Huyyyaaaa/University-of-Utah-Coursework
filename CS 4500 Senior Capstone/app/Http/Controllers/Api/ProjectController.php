<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\Project\ProjectIndexResource;
use App\Http\Resources\Project\ProjectShowResource;
use App\Models\ExtraCharge;
use App\Models\Project;

class ProjectController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', Project::class);

        return ProjectIndexResource::collection(team()->projects);
    }

    public function store()
    {
        $this->authorize('create', Project::class);

        $attributes         = $this->validateRequest();
        $computedAttributes = [
            'created_by_id' => user()->id,
        ];
        $result             = array_merge($attributes, $computedAttributes);

        $project = team()->projects()->create($result);
        $this->syncMaterials($project);
        $this->consolidateExtraCharges($project);
        return $this->success('Project Successfully Created', ['project' => ProjectShowResource::make($project)]);
    }

    public function show(Project $project)
    {
        $this->authorize('view', $project);

        return $this->success('Project Info', ['project' => ProjectShowResource::make($project)]);
    }

    public function update(Project $project)
    {
        $this->authorize('update', $project);

        $attributes = $this->validateRequest();
        $project->update($attributes);
        $this->syncMaterials($project);
        return $this->success('Project Successfully Updated', ['project' => ProjectShowResource::make($project)]);
    }

    public function destroy(Project $project)
    {
        $this->authorize('delete', $project);

        if ($project->bids->isNotEmpty()) {
            return $this->error('Cannot delete this project because it would delete the bids also.');
        }

        $project->materials()->detach();
        $project->delete();
        return $this->success('Project Successfully Deleted');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'name'              => 'required',
            'description'       => 'required',
            'address'           => 'required',
            'city'              => 'required',
            'state'             => 'required',
            'zip'               => 'required',
            'contact_name'      => 'required',
            'contact_phone'     => 'required',
            'contact_email'     => 'required',
            'sq_foot'           => 'required|numeric',
            'labor_hours'       => 'required|numeric',
            'labor_rate'        => 'required|numeric',
            'percentage_on_top' => 'required|numeric',
        ]);

        return $attributes;
    }

    private function syncMaterials(Project $project)
    {
        $materials = collect(request('materials'));
        $materials = $materials->mapWithKeys(function ($material) {
            return [
                $material['id'] => ['quantity' => $material['quantity']]
            ];
        });
        $project->materials()->sync($materials);
    }

    private function consolidateExtraCharges(Project $project)
    {
        $extraCharges = collect(request('extra_charges'));
        $extraCharges->each(function ($extraCharge) use ($project) {
            if (isset($extraCharge['id'])) {
                ExtraCharge::find($extraCharge['id'])->update([
                    'description' => $extraCharge['description'],
                    'amount'      => $extraCharge['amount'],
                ]);
            } else {
                $project->extraCharges()->create([
                    'team_id'     => team()->id,
                    'description' => $extraCharge['description'],
                    'amount'      => $extraCharge['amount'],
                ]);
            }
        });
    }
}
