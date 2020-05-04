<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateBidMaterialTable extends Migration
{
    public function up()
    {
        Schema::create('bid_material', function (Blueprint $table) {
            $table->unsignedBigInteger('bid_id');
            $table->unsignedBigInteger('material_id');
            $table->unsignedBigInteger('quantity');

            $table->foreign('bid_id')->references('id')->on('bids');
            $table->foreign('material_id')->references('id')->on('materials');
        });
    }

    public function down()
    {
        Schema::dropIfExists('bid_material');
    }
}
