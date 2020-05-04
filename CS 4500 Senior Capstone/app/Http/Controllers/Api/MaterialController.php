<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\Material\MaterialIndexResource;
use App\Http\Resources\Material\MaterialShowResource;
use App\Models\Material;

class MaterialController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', Material::class);

        return MaterialIndexResource::collection(team()->materials);
    }

    public function store()
    {
        $this->authorize('create', Material::class);

        $attributes         = $this->validateRequest();
        $computedAttributes = [
            'created_by_id' => user()->id,
        ];
        $result             = array_merge($attributes, $computedAttributes);

        $material = team()->materials()->create($result);
        return $this->success('Material Successfully Created', ['material' => MaterialShowResource::make($material)]);
    }

    public function update(Material $material)
    {
        $this->authorize('update', $material);

        $attributes = $this->validateRequest();
        $material->update($attributes);
        return $this->success('Material Successfully Updated', ['material' => MaterialShowResource::make($material)]);
    }

    public function destroy(Material $material)
    {
        $this->authorize('delete', $material);

        if ($material->projectTemplates->isNotEmpty() || $material->projects->isNotEmpty() || $material->bids->isNotEmpty()) {
            return $this->error('Cannot delete this material because it is being used by either a project template, project, or in a bid.');
        }

        $material->delete();
        return $this->success('Material Successfully Deleted');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'name'             => 'required',
            'description'      => 'required',
            'price'            => 'required|numeric',
            'sq_foot_coverage' => 'required|numeric',
        ]);

        return $attributes;
    }
}
