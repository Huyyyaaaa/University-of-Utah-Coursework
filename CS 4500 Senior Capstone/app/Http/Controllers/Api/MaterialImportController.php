<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Resources\Material\MaterialIndexResource;
use App\Imports\MaterialsImport;
use App\Models\Material;
use Maatwebsite\Excel\Facades\Excel;

class MaterialImportController extends Controller
{
    public function __invoke()
    {
        $this->authorize('create', Material::class);

        //Todo return $this->error with message if there was an error importing the file
        Excel::import(new MaterialsImport, request()->file('file'));

        return $this->success('File Successfully Uploaded!', [
            'materials' => MaterialIndexResource::collection(team()->materials),
        ]);
    }
}
