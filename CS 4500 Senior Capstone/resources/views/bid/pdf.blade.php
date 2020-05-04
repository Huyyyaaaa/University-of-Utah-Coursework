<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>{{$bid->name}}</title>
    <style>
        .clearfix:after {
            content: "";
            display: table;
            clear: both;
        }

        a {
            color: #4e73df;
            text-decoration: underline;
        }

        body {
            width: 100%;
            margin: 0 auto;
            font-size: 12px;
            font-family: Verdana, Geneva, sans-serif;
        }

        header {
            padding: 10px 0;
            margin-bottom: 10px;
        }

        #logo {
            text-align: center;
            margin-bottom: 10px;
        }

        #logo img {
            margin-top: -20px;
            height: 120px;
            width: 120px;
            object-fit: contain;
        }

        h1 {
            border-top: 1px solid #5D6975;
            border-bottom: 1px solid #5D6975;
            color: #5D6975;
            font-size: 2em;
            line-height: 1em;
            font-weight: normal;
            text-align: center;
            margin: 0 0 20px 0;
        }

        #project {
            float: left;
        }

        #company {
            float: right;
            /* text-align: right; */
        }

        #project div,
        #company div {
            white-space: nowrap;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            border-spacing: 0;
            margin-bottom: 20px;
        }

        table tr:nth-child(2n-1) td {
            background: #F5F5F5;
        }

        table th,
        table td {
            text-align: center;
        }

        table th {
            padding: 10px 10px;
            color: white;
            border-bottom: 1px solid #4e73df;
            white-space: nowrap;
            font-weight: bold;
            background-color: #4e73df;
        }

        table td {
            padding: 15px;
        }

        table td.desc {
            vertical-align: top;
        }

        table td.grand {
            border-top: 1px solid #5D6975;;
        }

        #notices .notice {
            color: #4e73df;
            font-size: 1.2em;
        }

        .row {
            width: 100%;
            min-height: 100px;
            overflow: auto;
        }

        hr {
            width: 150%;
            margin-left: -50px;
            border: none;
            height: .01em;
            background-color: rgba(78, 115, 223, 0.2);
        }
    </style>
</head>
<body>
<header class="clearfix">
    <div class="row">
        <!-- <div id="logo">
            <img src="{{ team()->photo_url }}" style="float: left;">
        </div> -->
        <div style="float: right;">
            <div><b>Created on: </b>{{date_format($bid->created_at, 'M j, Y')}}</div>
            <div><b>Due
                    on: </b>{{date_format(date_add($bid->created_at, date_interval_create_from_date_string('30 days')), 'M j, Y')}}
            </div>
        </div>
    </div>
    <hr>
    <div class="row">
        <div id="company">
            <div>{{$bid->team->name}}</div>
            <div>{{$bid->team->address}},<br/> {{$bid->team->city}}, {{$bid->team->state}} {{$bid->team->zip}}</div>
            <div>{{$bid->team->phone}}</div>
            <div><a href="mailto:{{$bid->team->team_email}}">{{$bid->team->team_email}}</a></div>
        </div>
        <div id="project">
            <div><b>Client Information</b></div>
            <div>{{$bid->contact_name}}</div>
            <div>{{$bid->address}}</div>
            <div> {{$bid->city}}, {{$bid->state}} {{$bid->zip}}</div>
            <div>{{$bid->contact_phone}}</div>
            <div><a href="mailto:{{$bid->contact_email}}">{{$bid->contact_email}}</a></div>
        </div>
    </div>
    <hr>
    <div class="row">
        <div><b>Project Description</b></div>
        <div>{{$bid->project->description}}</div>
        <br>
        <div><b>Project Sqft. </b>{{$bid->sq_foot}}</div>
    </div>
</header>
<main>
    <table>
        <thead>
        <tr>
            <th>#</th>
            <th>DESCRIPTION</th>
            <th>PRICE</th>
            <th>QTY</th>
            <th>TOTAL</th>
        </tr>
        </thead>
        <tbody>
        <?php
        $i = 1;
        $totalAmountMaterials = 0;
        ?>
        @foreach($bid->materials as $mat)
            <tr>
                <td class="service">{{$i}}</td>
                <td class="desc">{{$mat->name}}</td>
                <td class="unit">${{number_format($mat->price, 2, '.', ',')}}</td>
                <td class="qty">{{$mat->pivot->quantity}}</td>
                <td class="total">${{number_format($mat->price * $mat->pivot->quantity,2, '.', ',')}}</td>
            </tr>
            <?php
            $i++;
            $totalAmountMaterials += $mat->price * $mat->pivot->quantity;
            ?>
        @endforeach
        
        @foreach($bid->extraCharges as $ec)
            <tr>
                <td class="service">{{$i}}</td>
                <td class="desc">{{$ec->description}}</td>
                <td class="unit">${{number_format($ec->amount, 2, '.', ',')}}</td>
                <td class="qty">--</td>
                <td class="total">${{number_format($ec->amount, 2, '.', ',')}}</td>
            </tr>
            <?php $i++; ?>
        @endforeach
        <tr>
            <td class="service">{{$i}}</td>
            <td class="desc">Labor</td>
            <td class="unit">${{number_format($bid->labor_rate,2, '.', ',')}}/per hr</td>
            <td class="qty">{{$bid->labor_hours}} hrs</td>
            <td class="total">${{number_format($bid->labor_rate * $bid->labor_hours,2, '.', ',')}}</td>
        </tr>
        <?php $i++; ?>
        <tr>
            <td class="service">{{$i}}</td>
            <td class="desc">Miscellaneous</td>
            <td class="unit">--</td>
            <td class="qty">--</td>
            <td class="total">${{number_format($totalAmountMaterials * ($bid->percentage_on_top/100),2, '.', ',')}}</td>
        </tr>

        <tr>
            <td colspan="4" class="grand total"><b>GRAND TOTAL</b></td>
            <td class="grand total">${{number_format($bid->price,2, '.', ',')}}</td>
        </tr>
        </tbody>
    </table>
    <div id="notices">
        <div><b>Disclaimer:</b></div>
        <div class="notice">{{$bid->team->disclaimer}}</div>
    </div>
</main>
</body>
</html>
