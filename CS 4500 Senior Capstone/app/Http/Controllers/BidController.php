<?php

namespace App\Http\Controllers;

use App\Http\Resources\Bid\BidPdfResource;
use App\Http\Resources\Bid\BidShowResource;
use App\Http\Resources\ExtraChargeResource;
use App\Models\Bid;
use Barryvdh\DomPDF\Facade as PDF;

class BidController extends Controller
{
    public function show(Bid $bid)
    {
        $this->authorize('view', $bid);

        return view('bid.show', [
            'bid' => BidShowResource::make($bid)
        ]);
    }

    public function downloadPdf(Bid $bid)
    {
        $this->authorize('view', $bid);

        $pdf = PDF::loadView('bid.pdf', [
            'bid'           => BidPdfResource::make($bid),
            'extraCharges' => ExtraChargeResource::collection($bid->extraCharges),
        ]);
        return $pdf->download($bid->name . '.pdf');
    }
}
