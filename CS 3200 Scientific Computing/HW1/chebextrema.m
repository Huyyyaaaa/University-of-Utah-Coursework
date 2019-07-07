function ye = chebextrema(start, last, number )
    
    points = zeros(number, 1);
    
    for i = 1 : number
 
        if i == 1
            points(1) = start;
            
        elseif i == number
            points(i) = last;
        else
            n = (((i-1)*pi)/(number-1));
            points(i) = cos(n);
        end
        
    end
    
    ye = points;
    
  return
end
