<?php

namespace App\Http\Controllers;

use App\Exports\MaterialsExport;
use App\Models\Material;
use Maatwebsite\Excel\Facades\Excel;

class MaterialExportController extends Controller
{
    public function __invoke(Material $material)
    {
        $this->authorize('viewAny', Material::class);

        return Excel::download(new MaterialsExport, 'materials.csv');
    }
}
