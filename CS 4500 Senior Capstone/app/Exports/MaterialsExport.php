<?php

namespace App\Exports;

use Maatwebsite\Excel\Concerns\FromCollection;
use Maatwebsite\Excel\Concerns\WithHeadings;

class MaterialsExport implements FromCollection, WithHeadings
{
    public function headings(): array
    {
        return [
            'Name',
            'Description',
            'Price',
            'Sq Foot Coverage',
        ];
    }

    public function collection()
    {
        return team()->materials()->select(['name', 'description', 'price', 'sq_foot_coverage'])->get();
    }
}
