<?php

namespace App\Imports;

use App\Models\Material;
use Maatwebsite\Excel\Concerns\ToModel;
use Maatwebsite\Excel\Concerns\WithHeadingRow;

class MaterialsImport implements ToModel, WithHeadingRow
{
    public function model(array $row)
    {
        return new Material([
            'team_id'          => team()->id,
            'name'             => $row['name'],
            'description'      => $row['description'],
            'price'            => $row['price'],
            'sq_foot_coverage' => $row['sq_foot_coverage'],
            'created_by_id'    => user()->id,
        ]);
    }
}
