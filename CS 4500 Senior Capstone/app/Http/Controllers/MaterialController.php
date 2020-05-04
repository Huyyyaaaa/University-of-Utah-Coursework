<?php

namespace App\Http\Controllers;

use App\Http\Resources\Material\MaterialShowResource;
use App\Models\Material;

class MaterialController extends Controller
{
    public function index()
    {
        $this->authorize('viewAny', Material::class);

        return view('material.index');
    }

    public function create()
    {
        $this->authorize('create', Material::class);

        return view('material.create');
    }

    public function show(Material $material)
    {
        $this->authorize('view', $material);

        return view('material.show', [
            'material' => MaterialShowResource::make($material)
        ]);
    }
}
